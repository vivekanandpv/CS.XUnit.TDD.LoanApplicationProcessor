namespace LoanApplicationProcessor.Domain
{
    public class FinancialInstitution
    {
        private readonly List<LoanApplication> _applications = new List<LoanApplication>();

        public IEnumerable<LoanApplication> GetAllApplications()
        {
            return _applications;
        }

        public IEnumerable<LoanApplication> GetSubmittedApplications()
        {
            return _applications.Where(a => a.Status == ApplicationStatus.Submitted);
        }

        public void Submit(LoanApplication application)
        {
            _applications.Add(application);
        }

        public IEnumerable<LoanApplication> GetPendingVerificationApplications()
        {
            return _applications.Where(a => a.Status == ApplicationStatus.PendingVerification);
        }

        public IEnumerable<LoanApplication> GetVerificationFailedApplications()
        {
            return _applications.Where(a => a.Status == ApplicationStatus.VerificationFailed);
        }

        public IEnumerable<LoanApplication> GetApprovedApplications()
        {
            return _applications.Where(a => a.Status == ApplicationStatus.Approved);
        }
    }

    public class LoanApplication
    {
        public ApplicationStatus Status { get; set; }
    }

    public enum ApplicationStatus
    {
        Submitted,
        Resumbitted,
        PendingVerification,
        VerificationSuccess,
        VerificationFailed,
        PendingApproval,
        Approved,
        Rejected
    }
}