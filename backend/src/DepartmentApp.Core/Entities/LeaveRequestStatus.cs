namespace DepartmentApp.Entities
{
    public enum LeaveRequestStatus
    {
        Draft = 0,
        PendingManagerApproval = 1,
        PendingHRApproval = 2,
        Approved = 3,
        Revision = 4,
        Cancelled = 5,
        Rejected = 6,
    }
}