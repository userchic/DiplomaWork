
using Aspose.Words;
using Aspose.Words.Reporting;
using DiplomaWebApp.Abstractions;
using DiplomaWebApp.Models;
using DiplomaWebApp.Records;
using DiplomaWebApp.Repositories;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MimeKit;
using System.Reflection.Metadata.Ecma335;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DiplomaWebApp.Controllers
{
    [Authorize(AuthenticationSchemes = "Cookies")]
    [ApiController]
    [Route("/[controller]/[action]")]
    [EnableCors("AllowOneOrigin")]
    public class GameController : Controller
    {
        IJureRepository jureRep;
        IGameRepository gameRep;
        IRoundRepository roundRep;
        BreakRepository breakRep;
        RoleRepository roleRep;
        ChangeRepository changeRep;
        RolesChangesRepository rolesChangeRep;
        MistakeRepository mistakeRep;
        RoundResultRepository roundResultRep;
        public GameController(IGameRepository gamerep, IJureRepository jurerep, IRoundRepository roundrep, BreakRepository breakrep, RoleRepository rolerep, ChangeRepository changerep, RolesChangesRepository roleschangesrep, MistakeRepository mistakerep, RoundResultRepository roundResultrepository)
        {
            gameRep = gamerep;
            jureRep = jurerep;
            roundRep = roundrep;
            breakRep = breakrep;
            roleRep = rolerep;
            changeRep = changerep;
            rolesChangeRep = roleschangesrep;
            mistakeRep = mistakerep;
            roundResultRep = roundResultrepository;
        }
        [HttpGet]
        public IActionResult GetGame(int gameId)
        {
            Game targetGame = gameRep.GetGame(gameId);
            if (targetGame is null)
            {
                return NotFound();
            }
            return Json(JsonSerializer.Serialize(targetGame, new JsonSerializerOptions { ReferenceHandler = ReferenceHandler.Preserve }));
        }
        [HttpPost]
        public IActionResult ConfirmStart(int gameId)
        {
            Game targetGame = gameRep.GetGame(gameId);
            if (targetGame is null)
            {
                return NotFound();
            }
            Jure requestingUser = jureRep.GetJure(HttpContext.User.Identity.Name);
            if (!targetGame.Ongoing())
            {
                targetGame.Assessor = requestingUser;
                targetGame.StartTime = DateTime.UtcNow;
                gameRep.Save();
                return Json(new { success = 1, message = "Игра успешно начата" });
            }
            else
            {
                return Json(new { success = 0, message = "Игра уже проходит" });
            }
        }
        [HttpGet]
        public IActionResult DownloadTasks(int gameId)
        {
            Game targetGame = gameRep.GetGame(gameId);
            if (targetGame is null)
            {
                return NotFound();
            }
            Jure requestingUser = jureRep.GetJure(HttpContext.User.Identity.Name);
            if (targetGame.Assessor.Equals(requestingUser) && targetGame.Ongoing())
            {
                List<Problem> tasks = gameRep.GetGame(gameId).Tasks.ToList();
                Document tasksFile = FormTasksFile(tasks, gameId.ToString());
                FileStream stream = new FileStream($"tasks{gameId.ToString()}.docx", FileMode.Open, FileAccess.Read);
                string fileType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                return File(stream, fileType, $"tasks{gameId.ToString()}.docx");
            }
            else
            {
                return Json(new { success = 0, message = "Игра не проходит в данный момент или её проводит кто то другой" });
            }
        }
        [HttpPost]
        public IActionResult SendTasks(int gameId)
        {
            Game currentGame = gameRep.GetGame(gameId);
            if (currentGame is null)
            {
                return NotFound();
            }
            Jure requestingUser = jureRep.GetJure(HttpContext.User.Identity.Name);
            if (currentGame.Ongoing() && currentGame.Assessor.Equals(requestingUser))
            {
                List<Problem> tasks = currentGame.Tasks.ToList();
                Document tasksFile = FormTasksFile(tasks, gameId.ToString());
                foreach (Student student in currentGame.Team1.Students)
                {
                    if (!string.IsNullOrEmpty(student.Email))
                    {
                        sendEmailMessageWithFile(student.Email, $"tasks{gameId.ToString()}.docx");
                    }
                }
                return Json(new { success = 1, message = "Задачи успешно отправлены игрокам на Email" });
            }
            return Json(new { success = 0, message = "Игра не проходит в данный момент или её проводит кто то другой" });
        }
        [HttpPost]
        
        public IActionResult SetCaptainsRoundWinner(int gameId, int winnerTeamId)
        {
            Game currentGame = gameRep.GetGame(gameId);
            if (currentGame is null)
            {
                return NotFound();
            }
            Jure requestingUser = jureRep.GetJure(HttpContext.User.Identity.Name);
            if (currentGame.Ongoing() && currentGame.Assessor.Equals(requestingUser))
            {
                if (winnerTeamId != currentGame.Team1Id && winnerTeamId != currentGame.Team2Id)
                {
                    return Json(new { success = 0, message = "Указан несуществующий победитель" });
                }
                if (currentGame.CaptainsRound is null)
                {
                    CaptainsRound round = new CaptainsRound()
                    {
                        GameId = currentGame.Id,
                        Participant1Id = (int)currentGame.Team1.CaptainId,
                        Participant2Id = (int)currentGame.Team2.CaptainId,
                        WinnerId = winnerTeamId
                    };
                    currentGame.ChallengingTeamId = winnerTeamId;
                    currentGame.CaptainsRound = round;
                    gameRep.Save();
                    return Json(new { success = 1, message = "Успешно сохранена информация о результатах капитанского раунда" });
                }
                return Json(new { success = 0, message = "Информация о результатах капитанского раунда уже хранится" });
            }
            else
                return Json(new { success = 0, message = "Игра не идёт в данный момент или проводится другим членом жюри" });
        }
        [HttpPost]
        public IActionResult ConfirmSolvingStart(int gameId)
        {
            Game currentGame = gameRep.GetGame(gameId);
            if (currentGame is null)
            {
                return NotFound();
            }
            Jure requestingUser = jureRep.GetJure(HttpContext.User.Identity.Name);
            if (currentGame.Ongoing() && currentGame.Assessor.Equals(requestingUser))
            {
                if (currentGame.TaskSolvingStartTime != null)
                {
                    return Json(new { success = 0, message = "Начало решения задач уже зафиксировано" });
                }
                currentGame.TaskSolvingStartTime = DateTime.UtcNow;
                gameRep.Save();
                return Json(new { success = 1, time = currentGame.SolvingTime });
            }
            else
            {
                return Json(new { success = 0, message = "Игра не проходит в данный момент или её проводит кто-то другой" });
            }
        }
        
        [HttpGet]
        public IActionResult GetRounds(int gameId)
        {
            Game currentGame = gameRep.GetGame(gameId);
            if (currentGame is null)
            {
                return NotFound();
            }
            return Json(JsonSerializer.Serialize(currentGame.Challenges.Select(x => x.Round)));
        }
        [HttpPost]
        public IActionResult FixateChallenge(int gameId, int taskId)
        {
            Jure requestingUser = jureRep.GetJure(HttpContext.User.Identity.Name);
            Game currentGame = gameRep.GetGame(gameId);
            if (currentGame is null)
            {
                return NotFound();
            }
            if (!currentGame.Ongoing())
            {
                return Json(new { success = 0, message = "Игра не проходит в данный момент или уже закончилась" });
            }
            if (!currentGame.Assessor.Equals(requestingUser))
            {
                return Json(new { success = 0, message = "Вы не проводите эту игру" });
            }
            if (currentGame.CaptainsRound is null)
            {
                return Json(new { success = 0, message = "Вы не указали результаты капитанского раунда" });
            }
            if (!currentGame.IsSolvingHasEnded())
            {
                return Json(new { success = 0, message = "Решение задач еще не завершено" });
            }
            if (currentGame.Challenges.Count == currentGame.Tasks.Count)
            {
                return Json(new { success = 0, message = "Количество раундов соотетствует количеству задач, нужно завершить игру" });
            }
            if (currentGame.Challenges.Any(x => x.TaskId == taskId))
            {
                return Json(new { success = 0, message = "Эта задача уже была рассмотрена" });
            }
            if (currentGame.Challenges.Count > 0)
            {
                Challenge LastChallenge = currentGame.Challenges.Last();
                if (currentGame.Challenges.Count > 0 && (LastChallenge.Round == null || LastChallenge.Round.RoundResults == null))
                {
                    return Json(new { success = 0, message = "Текущий раунд еще не завершен" });
                }
            }
            currentGame.Challenges.Add(new Challenge()
            {
                GameId = gameId,
                DeclareTime = DateTime.UtcNow,
                RequestingTeamId = currentGame.ChallengingTeamId.Value,
                TaskId = taskId,
            });
            gameRep.Save();
            return Json(new { success = 1, message = "успешно объявлен вызов" });
        }
        [HttpPost]
        public IActionResult RejectToChallenge(int gameId)
        {
            Jure requestingUser = jureRep.GetJure(HttpContext.User.Identity.Name);
            Game currentGame = gameRep.GetGame(gameId);
            if (currentGame is null)
            {
                return NotFound();
            }
            if (!currentGame.Ongoing())
            {
                return Json(new { success = 0, message = "Игра не проходит в данный момент или уже закончилась" });
            }
            if (!currentGame.Assessor.Equals(requestingUser))
            {
                return Json(new { success = 0, message = "Вы не проводите эту игру" });
            }
            if (currentGame.CaptainsRound is null)
            {
                return Json(new { success = 0, message = "Вы не указали результаты капитанского раунда" });
            }
            if (!currentGame.IsSolvingHasEnded())
            {
                return Json(new { success = 0, message = "Решение задач еще не завершено" });
            }
            if (currentGame.Challenges.Count > 0)
            {
                Challenge LastChallenge = currentGame.Challenges.Last();
                if (currentGame.Challenges.Count > 0 && (LastChallenge.Round.RoundResults == null || LastChallenge.Round == null))
                {
                    return Json(new { success = 0, message = "Текущий раунд еще не завершен" });
                }
            }
            if (currentGame.TeamRejectedToChallenge == true)
            {
                return EndGame(gameId);
            }
            currentGame.ChallengingTeamId = currentGame.Team2Id + currentGame.Team1Id - currentGame.ChallengingTeamId;
            currentGame.TeamRejectedToChallenge = true;
            gameRep.Save();
            return Json(new { success = 1, message = "Изменен порядок игры, теперь выступать будет только одна команда. Если она откажется выступать то игра закончится." });
        }
        [HttpPost]
        public IActionResult ConfirmCorrectnessCheck(int gameId)
        {
            Jure requestingUser = jureRep.GetJure(HttpContext.User.Identity.Name);
            Game currentGame = gameRep.GetGame(gameId);
            if (currentGame is null)
            {
                return NotFound();
            }
            if (!currentGame.Ongoing())
            {
                return Json(new { success = 0, message = "Игра не проходит в данный момент или уже закончилась" });
            }
            if (!currentGame.Assessor.Equals(requestingUser))
            {
                return Json(new { success = 0, message = "Вы не проводите эту игру" });
            }
            if (currentGame.CaptainsRound is null)
            {
                return Json(new { success = 0, message = "Вы не указали результаты капитанского раунда" });
            }
            if (!currentGame.IsSolvingHasEnded())
            {
                return Json(new { success = 0, message = "Решение задач еще не завершено" });
            }
            if (currentGame.Challenges.Count == 0)
            {
                return Json(new { success = 0, message = "Никакого вызова не объявлено" });
            }
            if (currentGame.Challenges.Last().Round != null)
            {
                if (currentGame.Challenges.Last().Round.RoundResults == null)
                    return Json(new { success = 0, message = "Текущий раунд не завершен" });
                else
                    return Json(new { success = 0, message = "Нет необработанного вызова" });
            }
            if (currentGame.Challenges.Last().IsChallengeAccepted || currentGame.Challenges.Last().IsCheckingCorrectness)
            {
                return Json(new { success = 0, message = "Вызов уже был принят, отвергнут или была зафиксирована проверка корректности" });
            }
            currentGame.Challenges.Last().IsCheckingCorrectness = true;
            gameRep.Save();
            return Json(new { success = 1, message = "Зафиксирована проверка корректности" });
        }
        [HttpPost]
        public IActionResult ConfirmChallengeAcceptance(int gameId)
        {
            Jure requestingUser = jureRep.GetJure(HttpContext.User.Identity.Name);
            Game currentGame = gameRep.GetGame(gameId);
            if (currentGame is null)
            {
                return NotFound();
            }
            if (!currentGame.Ongoing())
            {
                return Json(new { success = 0, message = "Игра не проходит в данный момент или уже закончилась" });
            }
            if (!currentGame.Assessor.Equals(requestingUser))
            {
                return Json(new { success = 0, message = "Вы не проводите эту игру" });
            }
            if (currentGame.CaptainsRound is null)
            {
                return Json(new { success = 0, message = "Вы не указали результаты капитанского раунда" });
            }
            if (!currentGame.IsSolvingHasEnded())
            {
                return Json(new { success = 0, message = "Решение задач еще не завершено" });
            }
            if (currentGame.Challenges.Count == 0)
            {
                return Json(new { success = 0, message = "Никакого вызова не объявлено" });
            }
            if (currentGame.Challenges.Last().Round != null)
            {
                if (currentGame.Challenges.Last().Round.RoundResults == null)
                    return Json(new { success = 0, message = "Текущий раунд не завершен" });
                else
                    return Json(new { success = 0, message = "Нет необработанного вызова" });
            }
            if (currentGame.Challenges.Last().IsChallengeAccepted|| currentGame.Challenges.Last().IsCheckingCorrectness)
            {
                return Json(new { success = 0, message = "Вызов уже был принят, отвергнут или была зафиксирована проверка корректности" });
            }
            currentGame.Challenges.Last().IsChallengeAccepted = true;
            gameRep.Save();
            return Json(new { success = 1, message = "Зафиксировано принятие вызова" });
        }
        [HttpPost]
        public IActionResult StartNewRound(RoundRecord round)
        {
            //TODO проверка что человек выступал менее 2 раз
            if (!ModelState.IsValid)
            {
                return JsonFirstError();
            }
            Jure requestingUser = jureRep.GetJure(HttpContext.User.Identity.Name);
            Game currentGame = gameRep.GetGame(round.GameId);
            if (currentGame is null)
            {
                return NotFound();
            }
            if (!currentGame.Ongoing())
            {
                return Json(new { success = 0, message = "Игра не проходит в данный момент или уже закончилась" });
            }
            if (!currentGame.Assessor.Equals(requestingUser))
            {
                return Json(new { success = 0, message = "Вы не проводите эту игру" });
            }
            if (currentGame.CaptainsRound is null)
            {
                return Json(new { success = 0, message = "Вы не указали результаты капитанского раунда" });
            }
            if (currentGame.Challenges.Count > 0)
            {
                if (!currentGame.Challenges.Last().IsChallengeAccepted && !currentGame.Challenges.Last().IsCheckingCorrectness)
                {
                    return Json(new { success = 0, message = "Последний вызов не был обработан. Зафиксируйте принятие, отвержение вызова или проверку корректности" });
                }
                if (currentGame.Challenges.Last().Round is not null)
                {
                    return Json(new { success = 0, message = "Нет вызова по которому необходимо начать раунд" });
                }
            }
            Round newRound = new Round()
            {
                OpponentId = round.OpponentId,
                SpeakerId = round.SpeakerId,
                StartTime = DateTime.UtcNow,
                ChallengeId = currentGame.Challenges.Last().Id,
                RoundNumber = currentGame.Challenges.Count,
            };
            roundRep.AddRound(newRound);
            roundRep.Save();
            currentGame.Challenges.Last().Round = newRound;
            gameRep.Save();
            return Json(new { success = 1, message = "Успешно начат новый раунд" });
        }
        [HttpPost]
        public IActionResult EndRound(int gameId, EndRoundRecord record)
        {
            if (!ModelState.IsValid)
            {
                return JsonFirstError();
            }
            Jure requestingUser = jureRep.GetJure(HttpContext.User.Identity.Name);
            Game currentGame = gameRep.GetGame(gameId);
            if (currentGame is null)
            {
                return NotFound();
            }
            if (!currentGame.Assessor.Equals(requestingUser))
            {
                return Json(new { success = 0, message = "Вы не проводите эту игру" });
            }
            if (!currentGame.Ongoing())
            {
                return Json(new { success = 0, message = "Игра не проходит в данный момент или уже закончилась" });
            }
            if (currentGame.Challenges.Count > 0 && currentGame.Challenges.Last().Round is not null && currentGame.Challenges.Last().Round.RoundResults is not null)
            {
                return Json(new { success = 0, message = "Текущий раунд уже завершен" });
            }
            bool Correctness = true;
            if (currentGame.Challenges.Last().IsCheckingCorrectness && currentGame.Challenges.Last().Round.NoSolution)
            {
                Correctness = false;
            }
            RoundResults newRes = new RoundResults()
            {
                Correctness = Correctness,
                RoundEndTime = DateTime.UtcNow,
                Team1Points = record.Team1Points,
                Team2Points = record.Team2Points,
                RoundId = currentGame.Challenges.Last().Round.Id
            };
            currentGame.Team1Points += record.Team1Points;
            currentGame.Team2Points += record.Team2Points;


            if (newRes.Correctness && !currentGame.TeamRejectedToChallenge)
            {
                currentGame.ChallengingTeamId = currentGame.Team2Id + currentGame.Team1Id - currentGame.ChallengingTeamId;
            }
            roundResultRep.AddResult(newRes);
            List<Mistake> newMistakes = record.Mistakes.Select(x => new Mistake(x)).ToList();
            newMistakes.ForEach(x => x.ResultsId = newRes.Id);
            mistakeRep.AddRangeResult(newMistakes);
            roundResultRep.Save();
            mistakeRep.Save();
            gameRep.Save();
            if (currentGame.Challenges.Count < currentGame.Tasks.Count)
                return Json(new { success = 1, message = "Успешно завершен раунд" });
            else
                return EndGame(gameId);
        }
        [HttpPost]
        public IActionResult EndGame(int gameId)
        {
            Jure requestingUser = jureRep.GetJure(HttpContext.User.Identity.Name);
            Game currentGame = gameRep.GetGame(gameId);
            if (!currentGame.Assessor.Equals(requestingUser))
            {
                return Json(new { success = 0, message = "Вы не проводите эту игру" });
            }
            if (!currentGame.Ongoing() || currentGame.GameEnded)
            {
                return Json(new { success = 0, message = "Игра не проходит в данный момент или уже закончилась" });
            }
            currentGame.GameEnded = true;
            gameRep.Save();
            return Json(new { success = 1, message = "Игра успешно закончена" });
        }
        [HttpPost]
        public IActionResult DeclareBreak(int gameId, int initiatorTeamId)
        {
            Game currentGame = gameRep.GetGame(gameId);
            if (currentGame is null)
            {
                return NotFound();
            }
            Jure requestingUser = jureRep.GetJure(HttpContext.User.Identity.Name);
            if (!currentGame.Assessor.Equals(requestingUser))
            {
                return Json(new { success = 0, message = "Вы не проводите эту игру" });
            }
            if (!currentGame.Ongoing())
            {
                return Json(new { success = 0, message = "Игра не проходит в данный момент или уже закончилась" });
            }
            if (currentGame.CaptainsRound is null)
            {
                return Json(new { success = 0, message = "Вы не указали результаты капитанского раунда" });
            }
            if (currentGame.Team1Id != initiatorTeamId && currentGame.Team2Id != initiatorTeamId)
            {
                return Json(new { success = 0, message = "Указана команда не участвующая в игре" });
            }
            Team initiatorTeam = currentGame.Team1Id == initiatorTeamId ? currentGame.Team1 : currentGame.Team2;
            int RequestedBreaksAndChangesCost = (initiatorTeam.Breaks is not null ? initiatorTeam.Breaks.Count : 0) + (initiatorTeam.Changes is not null ? initiatorTeam.Changes.Count * 2 : 0);
            if (RequestedBreaksAndChangesCost > 5)
            {
                return Json(new { success = 0, message = "Данная команда больше не может объявить перерыв" });
            }
            else
            {

                Break newBreak = new Break()
                {
                    DeclareTime = DateTime.UtcNow,
                    InitiatorTeamId = initiatorTeamId,
                    RoundId = currentGame.Challenges.Last().Round.Id
                };
                breakRep.AddBreak(gameId, newBreak);
                breakRep.Save();
                return Json(new { success = 1, message = "Успешно объявлен перерыв 30 секунд" });
            }
        }
        [HttpPost]
        public IActionResult DeclareChange(int gameId, int initiatorTeamId, int newParticipantId)
        {
            Game currentGame = gameRep.GetGame(gameId);
            if (currentGame is null)
            {
                return NotFound();
            }
            Jure requestingUser = jureRep.GetJure(HttpContext.User.Identity.Name);
            if (!currentGame.Assessor.Equals(requestingUser))
            {
                return Json(new { success = 0, message = "Вы не проводите эту игру" });
            }
            if (!currentGame.Ongoing())
            {
                return Json(new { success = 0, message = "Игра не проходит в данный момент или уже закончилась" });
            }
            if (currentGame.CaptainsRound is null)
            {
                return Json(new { success = 0, message = "Вы не указали результаты капитанского раунда" });
            }
            if (currentGame.Team1Id != initiatorTeamId && currentGame.Team2Id != initiatorTeamId)
            {
                return Json(new { success = 0, message = "Указана команда не участвующая в игре" });
            }
            Team initiatorTeam = currentGame.Team1Id == initiatorTeamId ? currentGame.Team1 : currentGame.Team2;
            //вычисление стоимости уже запрошенных данной командой перерывов и замен
            int RequestedBreaksAndChangesCost = (initiatorTeam.Breaks is not null ? initiatorTeam.Breaks.Count : 0) + (initiatorTeam.Changes is not null ? initiatorTeam.Changes.Count * 2 : 0);
            if (RequestedBreaksAndChangesCost > 4)
            {
                return Json(new { success = 0, message = "Данная команда больше не может производить замены выступающих" });
            }
            Round currentRound = currentGame.Challenges.Last().Round;
            //берем спикера и ищем его в указанной команде, если есть то берем его идентификатор. Иначе берем оппонента и ищем его в указанной команде. Если и его нет то возвращаем -1.
            int currentParticipantId = (int)(initiatorTeam.Students.Any(x => x.Id == currentRound.OpponentId) ? currentRound.OpponentId : initiatorTeam.Students.Any(x => x.Id == currentRound.SpeakerId) ? currentRound.SpeakerId : -1);
            if (!initiatorTeam.Students.Any(x => x.Id == newParticipantId) || !initiatorTeam.Students.Any(x => x.Id == currentParticipantId))
            {
                return Json(new { success = 0, message = "Указан студент не принадлежащий к указанной команде" });
            }
            else
            {
                Role newRole = currentGame.Challenges.Last().Round.OpponentId == currentParticipantId ? roleRep.GetRole(1) : roleRep.GetRole(2);
                Change newChange = new Change()
                {
                    DeclareTime = DateTime.UtcNow,
                    InitiatorTeamId = initiatorTeamId,
                    NewParticipantId = newParticipantId,
                    RoundId = currentGame.Challenges.Last().Round.Id,
                    RoleId = newRole.Id
                };
                changeRep.AddChange(newChange);
                return Json(new { success = 1, message = "Успешно добавлена запись о замене участника" });
            }
        }
        [HttpPost]
        public IActionResult DeclareRoleChange(int gameId, int isFullRoleChange)
        {
            Jure requestingUser = jureRep.GetJure(HttpContext.User.Identity.Name);
            Game currentGame = gameRep.GetGame(gameId);
            if (currentGame is null)
            {
                return NotFound();
            }
            if (!currentGame.Assessor.Equals(requestingUser))
            {
                return Json(new { success = 0, message = "Вы не проводите эту игру" });
            }
            if (!currentGame.Ongoing())
            {
                return Json(new { success = 0, message = "Игра не проходит в данный момент или уже закончилась" });
            }
            if (currentGame.CaptainsRound is null)
            {
                return Json(new { success = 0, message = "Вы не указали результаты капитанского раунда" });
            }

            if (currentGame.Challenges.Last().Round.RolesChange is not null)
            {
                return Json(new { success = 0, message = "В текущем раунде уже произошла перемена ролей" });
            }
            else
            {
                RolesChange newChange = new RolesChange()
                {
                    ChangeTime = DateTime.UtcNow,
                    FullRoleChange = isFullRoleChange==1,
                    RoundId = currentGame.Challenges.Last().Round.Id,
                };
                rolesChangeRep.AddChange(newChange);
                rolesChangeRep.Save();
                return Json(new { success = 1, message = "Вы успешно объявили перемену ролей" });
            }
        }
        [HttpPost]
        public IActionResult ConfirmNoSolution(int gameId)
        {
            Jure requestingUser = jureRep.GetJure(HttpContext.User.Identity.Name);
            Game currentGame = gameRep.GetGame(gameId);
            if (currentGame is null)
            {
                return NotFound();
            }
            if (!currentGame.Ongoing())
            {
                return Json(new { success = 0, message = "Игра не проходит в данный момент или уже закончилась" });
            }
            if (!currentGame.Assessor.Equals(requestingUser))
            {
                return Json(new { success = 0, message = "Вы не проводите эту игру" });
            }
            if (currentGame.CaptainsRound is null)
            {
                return Json(new { success = 0, message = "Вы не указали результаты капитанского раунда" });
            }
            if (currentGame.Challenges.Count > 0)
            {
                if (currentGame.Challenges.Last().Round is null)
                {
                    return Json(new { success = 0, message = "По последнему вызову еще не был начат раунд" });
                }
                if (currentGame.Challenges.Last().Round.RoundResults is not null)
                {
                    return Json(new { success = 0, message = "Последний начатый раунд уже завершен" });
                }
                if (currentGame.Challenges.Last().Round.NoSolution)
                {
                    return Json(new { success = 0, message = "Отсутствие решения уже было зафиксировано" });
                }
                currentGame.Challenges.Last().Round.NoSolution = true;
                gameRep.Save();
                return Json(new { success = 1, message = "Успешно зафиксировано отсутствие решения" });
            }
            else
            {
                return Json(new { success = 0, message = "Ни одного вызова еще не произошло" });
            }
        }
        private Document FormTasksFile(List<Problem> tasks, string name)
        {
            Document doc = new Document("./Templates/TasksTemplate.docx");
            ReportingEngine engine = new ReportingEngine();
            engine.BuildReport(doc, tasks, "Tasks");
            doc.Save($"tasks{name}.docx");
            return doc;
        }
        private void sendEmailMessageWithFile(string email, string fileName)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("MathBattlesSender", "Math.Battles@mail.ru"));
            message.To.Add(new MailboxAddress("TargetTeamMember", email));
            message.Subject = "Задачи математического боя от" + DateTime.Now.ToShortDateString;
            var builder = new BodyBuilder();
            builder.TextBody = "В данном сообщении содержится файл с задачами математического боя от " + DateTime.Now.ToShortDateString;
            builder.Attachments.Add(fileName);
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.mail.ru", 25, false);
                client.Authenticate("Math.Battles@mail.ru", "0komodetS-4nissan-8sardiNka");
                client.Send(message);
                client.Disconnect(true);
            }
        }
        private IActionResult JsonFirstError()
        {
            return Json(new { success = 0, message = ModelState.First(x => x.Value.Errors.Count > 0).Value.Errors.First().ErrorMessage });
        }
    }
}
