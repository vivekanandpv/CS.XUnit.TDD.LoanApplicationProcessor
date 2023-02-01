﻿using System.Text.RegularExpressions;

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

        public void VerifyApplication(int id, bool isVerified)
        {
            var application = _applications.First(a => a.Id == id);

            application.Status = isVerified ? ApplicationStatus.VerificationSuccess : ApplicationStatus.VerificationFailed;
        }

        public void ApproveApplication(int id)
        {
            var application = _applications.First(a => a.Id == id);

            application.Status = ApplicationStatus.Approved;
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
        PendingVerification,
        VerificationSuccess,
        VerificationFailed,
        PendingApproval,
        Approved,
        Rejected
    }
}