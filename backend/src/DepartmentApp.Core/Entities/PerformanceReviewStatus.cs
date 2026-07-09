namespace DepartmentApp.Entities
{
    public enum PerformanceReviewStatus
    {
        Draft = 0,
        SelfAssessmentPending = 1,
        ManagerReviewPending = 2,
        PeerReviewPending = 3,
        HRReviewPending = 4,
        Completed = 5,
        Cancelled = 6,
    }
}