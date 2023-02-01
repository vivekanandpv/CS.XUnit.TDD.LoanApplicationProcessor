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
            
            sut.Submit(new LoanApplication{Id = 1, Status = ApplicationStatus.Submitted, CreditScore = 750, Pan = "ABCDE1234F"});
            sut.Submit(new LoanApplication{Id = 2, Status = ApplicationStatus.Submitted, CreditScore = 760, Pan = "ABCDE1244F"});
            sut.Submit(new LoanApplication{Id = 3, Status = ApplicationStatus.Submitted, CreditScore = 770, Pan = "ABCDE1254F"});
            sut.Submit(new LoanApplication{Id = 4, Status = ApplicationStatus.Submitted, CreditScore = 780, Pan = "ABCDE1264F"});

            IEnumerable<LoanApplication> submittedApplications = sut.GetSubmittedApplications();
            Assert.All(
                submittedApplications,
                a =>
                {
                    Assert.True(a.Status == ApplicationStatus.Submitted);
                });

            Assert.Equal(4, submittedApplications.Count());
        }

        [Fact]
        public void ReturnSequenceOfAllPendingVerificationApplications()
        {
            FinancialInstitution sut = new FinancialInstitution();
            
            sut.Submit(new LoanApplication{Id = 1, Status = ApplicationStatus.Submitted, CreditScore = 750, Pan = "ABCDE1234F"});
            sut.Submit(new LoanApplication{Id = 2, Status = ApplicationStatus.Submitted, CreditScore = 760, Pan = "ABCDE1244F"});
            sut.Submit(new LoanApplication{Id = 3, Status = ApplicationStatus.Submitted, CreditScore = 770, Pan = "ABCDE1254F"});
            sut.Submit(new LoanApplication{Id = 4, Status = ApplicationStatus.Submitted, CreditScore = 780, Pan = "ABCDE1264F"});

            IEnumerable<LoanApplication> applications = sut.GetPendingVerificationApplications();
            Assert.All(
                applications,
                a =>
                {
                    Assert.True(a.Status == ApplicationStatus.Submitted || a.Status == ApplicationStatus.Resumbitted);
                });

            Assert.Equal(4, applications.Count());
        }

        [Fact]
        public void ReturnSequenceOfAllVerificationFailedApplications()
        {
            FinancialInstitution sut = new FinancialInstitution();
            
            sut.Submit(new LoanApplication{Id = 1, Status = ApplicationStatus.Submitted, CreditScore = 750, Pan = "ABCDE1234F"});
            sut.Submit(new LoanApplication{Id = 2, Status = ApplicationStatus.Submitted, CreditScore = 760, Pan = "ABCDE1244F"});
            sut.Submit(new LoanApplication{Id = 3, Status = ApplicationStatus.Submitted, CreditScore = 770, Pan = "ABCDE1254F"});
            sut.Submit(new LoanApplication{Id = 4, Status = ApplicationStatus.Submitted, CreditScore = 780, Pan = "ABCDE1264F"});

            sut.VerifyApplication(1, false);

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
            
            sut.Submit(new LoanApplication{Id = 1, Status = ApplicationStatus.Submitted, CreditScore = 750, Pan = "ABCDE1234F"});
            sut.Submit(new LoanApplication{Id = 2, Status = ApplicationStatus.Submitted, CreditScore = 760, Pan = "ABCDE1244F"});
            sut.Submit(new LoanApplication{Id = 3, Status = ApplicationStatus.Submitted, CreditScore = 770, Pan = "ABCDE1254F"});
            sut.Submit(new LoanApplication{Id = 4, Status = ApplicationStatus.Submitted, CreditScore = 780, Pan = "ABCDE1264F"});

            sut.ApproveApplication(2);

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

        [Theory]
        [MemberData(nameof(GetInvalidApplications))]
        public void ThrowArgumentExceptionForInvalidApplication(LoanApplication application)
        {
            FinancialInstitution sut = new FinancialInstitution();

            Assert.Throws<ArgumentException>(() =>
            {
                sut.Submit(application);
            });
        }

        public static IEnumerable<object[]> GetInvalidApplications()
        {
            return new List<object[]>
            {
                new object[]
                {
                    new LoanApplication
                    {
                        Status = ApplicationStatus.Approved, Name = "John Doe", Pan = "ABCDE1234F", CreditScore = 750
                    }
                },
                new object[]
                {
                    new LoanApplication
                    {
                        Status = ApplicationStatus.Submitted, Name = "John Doe", Pan = "ABCDDE1234F", CreditScore = 750
                    }
                },
                new object[]
                {
                    new LoanApplication
                    {
                        Status = ApplicationStatus.Submitted, Name = "John Doe", Pan = "ABCDE1234F", CreditScore = 999
                    }
                },
                new object[]
                {
                    new LoanApplication
                    {
                        Status = ApplicationStatus.Submitted, Name = "John Doe", Pan = "ABCDE1234F", CreditScore = 250
                    }
                },
            };
        }
    }
}