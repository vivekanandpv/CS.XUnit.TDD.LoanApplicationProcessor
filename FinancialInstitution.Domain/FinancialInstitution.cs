using System.Text.RegularExpressions;

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
            if (application.Status != ApplicationStatus.Submitted)
            {
                throw new ArgumentException("Invalid status");
            }

            if (application.CreditScore < 500 || application.CreditScore > 900)
            {
                throw new ArgumentException("Invalid credit score");
            }

            var panRegex = new Regex("^[A-Z]{5}[0-9]{4}[A-Z]$");
            if (!panRegex.IsMatch(application.Pan))
            {
                throw new ArgumentException("Invalid PAN");
            }

            _applications.Add(application);
        }

        public IEnumerable<LoanApplication> GetPendingVerificationApplications()
        {
            return _applications.Where(a => a.Status == ApplicationStatus.Submitted || a.Status == ApplicationStatus.Resumbitted);
        }

        public IEnumerable<LoanApplication> GetVerificationFailedApplications()
        {
            return _applications.Where(a => a.Status == ApplicationStatus.VerificationFailed);
        }

        public IEnumerable<LoanApplication> GetApprovedApplications()
        {
            return _applications.Where(a => a.Status == ApplicationStatus.Approved);
        }

        public void MarkVerificationSuccess()
        {
            var application = GetPendingVerificationApplication();

            application.Status = ApplicationStatus.VerificationSuccess;
        }

        public void MarkVerificationFailed()
        {
            var application = GetPendingVerificationApplication();

            application.Status = ApplicationStatus.VerificationFailed;
        }

        public void ApproveApplication()
        {
            var application = GetPendingApprovalApplication();

            application.Status = ApplicationStatus.Approved;
        }

        public void RejectApplication()
        {
            var application = GetPendingApprovalApplication();

            application.Status = ApplicationStatus.Rejected;
        }

        public void ResubmitApplication(int id)
        {
            var application = _applications.Where(a => a.Id == id && a.Status == ApplicationStatus.VerificationFailed)
                .First();

            application.Status = ApplicationStatus.Resumbitted;
        }

        private LoanApplication GetPendingVerificationApplication()
        {
            return _applications
                .Where(a => a.Status == ApplicationStatus.Submitted || a.Status == ApplicationStatus.Resumbitted)
                .OrderBy(a => a.Id).First();
        }

        private LoanApplication GetPendingApprovalApplication()
        {
            return _applications
                .Where(a => a.Status == ApplicationStatus.VerificationSuccess)
                .OrderBy(a => a.Id).First();
        }

        public IEnumerable<LoanApplication> GetResubmittedApplications()
        {
            return _applications
                .Where(a => a.Status == ApplicationStatus.Resumbitted);
        }

        public IEnumerable<LoanApplication> GetRejectedApplications()
        {
            return _applications
                .Where(a => a.Status == ApplicationStatus.Rejected);
        }
    }

    public class LoanApplication
    {
        public int Id { get; set; }
        public ApplicationStatus Status { get; set; }
        public string Name { get; set; }
        public string Pan { get; set; }
        public int CreditScore { get; set; }
    }

    public enum ApplicationStatus
    {
        Submitted,
        Resumbitted,
        VerificationSuccess,
        VerificationFailed,
        Approved,
        Rejected
    }
}