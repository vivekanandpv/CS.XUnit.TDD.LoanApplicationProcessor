using LoanApplicationProcessor.Domain;

namespace LoanApplicationProcessor.Tests
{
    public class FinancialInstitutionShould
    {
        [Fact]
        public void ReturnSequenceOfAllApplications()
        {
            FinancialInstitution sut = new FinancialInstitution();
            IEnumerable<LoanApplication> applications = sut.GetAllApplications();

            Assert.IsAssignableFrom<IEnumerable<LoanApplication>>(applications);
        }

        [Fact]
        public void ReturnSequenceOfAllSubmittedApplications()
        {
            FinancialInstitution sut = new FinancialInstitution();
            
            sut.Submit(new LoanApplication{Status = ApplicationStatus.Approved});
            sut.Submit(new LoanApplication{Status = ApplicationStatus.Submitted});
            sut.Submit(new LoanApplication{Status = ApplicationStatus.PendingVerification});
            sut.Submit(new LoanApplication{Status = ApplicationStatus.Submitted});

            IEnumerable<LoanApplication> submittedApplications = sut.GetSubmittedApplications();
            Assert.All(
                submittedApplications,
                a =>
                {
                    Assert.True(a.Status == ApplicationStatus.Submitted);
                });

            Assert.Equal(2, submittedApplications.Count());
        }

        [Fact]
        public void ReturnSequenceOfAllPendingVerificationApplications()
        {
            FinancialInstitution sut = new FinancialInstitution();
            
            sut.Submit(new LoanApplication{Status = ApplicationStatus.Approved});
            sut.Submit(new LoanApplication{Status = ApplicationStatus.Submitted});
            sut.Submit(new LoanApplication{Status = ApplicationStatus.PendingVerification});
            sut.Submit(new LoanApplication{Status = ApplicationStatus.Submitted});

            IEnumerable<LoanApplication> applications = sut.GetPendingVerificationApplications();
            Assert.All(
                applications,
                a =>
                {
                    Assert.True(a.Status == ApplicationStatus.PendingVerification);
                });

            Assert.Equal(1, applications.Count());
        }

        [Fact]
        public void ReturnSequenceOfAllVerificationFailedApplications()
        {
            FinancialInstitution sut = new FinancialInstitution();
            
            sut.Submit(new LoanApplication{Status = ApplicationStatus.Approved});
            sut.Submit(new LoanApplication{Status = ApplicationStatus.Submitted});
            sut.Submit(new LoanApplication{Status = ApplicationStatus.PendingVerification});
            sut.Submit(new LoanApplication{Status = ApplicationStatus.VerificationFailed});

            IEnumerable<LoanApplication> applications = sut.GetVerificationFailedApplications();
            Assert.All(
                applications,
                a =>
                {
                    Assert.True(a.Status == ApplicationStatus.VerificationFailed);
                });

            Assert.Equal(1, applications.Count());
        }

        [Fact]
        public void ReturnSequenceOfAllApprovedApplications()
        {
            FinancialInstitution sut = new FinancialInstitution();
            
            sut.Submit(new LoanApplication{Status = ApplicationStatus.Approved});
            sut.Submit(new LoanApplication{Status = ApplicationStatus.Submitted});
            sut.Submit(new LoanApplication{Status = ApplicationStatus.PendingVerification});
            sut.Submit(new LoanApplication{Status = ApplicationStatus.VerificationFailed});

            IEnumerable<LoanApplication> applications = sut.GetApprovedApplications();
            Assert.All(
                applications,
                a =>
                {
                    Assert.True(a.Status == ApplicationStatus.Approved);
                });

            Assert.Equal(1, applications.Count());
        }

        [Fact]
        public void SubmitValidApplication()
        {
            FinancialInstitution sut = new FinancialInstitution();

            sut.Submit(new LoanApplication{Status = ApplicationStatus.Submitted, Name = "John Doe", Pan = "ABCDE1234F", CreditScore = 750});

            Assert.Equal(1, sut.GetAllApplications().Count());
        }

        [Fact]
        public void ThrowArgumentExceptionForInvalidStatusApplication()
        {
            FinancialInstitution sut = new FinancialInstitution();

            Assert.Throws<ArgumentException>(() =>
            {
                sut.Submit(new LoanApplication
                    { Status = ApplicationStatus.Approved, Name = "John Doe", Pan = "ABCDE1234F", CreditScore = 750 });
            });
        }
    }
}