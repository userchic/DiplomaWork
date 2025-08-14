interface Props {
    setState: (newState: string) => void,
    ConfirmCorrectnessCheck: () => Promise<boolean>,
    ChallengeAccept: () => Promise<boolean>,
}
export default function ChallengeAcceptanceInterface({ setState, ConfirmCorrectnessCheck, ChallengeAccept }: Props) {

    async function handleCallAccept() {
        if (await ChallengeAccept())
            setState("ParticipantChoosing")
    }

    async function handleCheckingCorrectnessConfirmation() {
        if (await ConfirmCorrectnessCheck())
            setState("ParticipantChoosing")
    }

    return (
        <>
            <input type="button" value="Подтвердить принятие вызова" onClick={handleCallAccept} />
            <br />
            <input type="button" value="Зафиксировать проверку корректности" onClick={handleCheckingCorrectnessConfirmation} />
        </>
    )
}