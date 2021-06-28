namespace NameGame.Service.Models.Dto {
    public class ChallengeAnswerValidationResult {
        public ChallengeAnswerValidationResult() {
            Success = true;
        }

        public ChallengeAnswerValidationResult(string errorMessage) {
            ErrorMessage = errorMessage;
        }

        public bool Success { get; }
        public string ErrorMessage { get; }
    }
}
