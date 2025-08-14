interface Props {
    setState: (newState: string) => void,
    RejectToChallenge: () => void
}
export default function StartOneTeamPerformance({ setState, RejectToChallenge }: Props) {

    function handleContinueRounds(): void {
        setState("Challange")
    }

    function handleRejectToChallenge(): void {
        RejectToChallenge()
    }

    return (
        <>
            Противоположная команда желает рассказать решения каких то задач?
            <input type="button" value="Подтвердить желание противоположной команды рассказать еще некоторые задачи" onClick={handleContinueRounds} />
            <input type="button" value="Зафиксировать отказ команды от выступлений" onClick={handleRejectToChallenge} />
        </>
    )
}