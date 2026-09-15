namespace DepartmentApp.Entities
{
    public enum PurchaseRequestStatus
    {
        Draft = 0,
        PendingManagerApproval = 1,
        PendingFinanceApproval = 2,
        PendingDirectorApproval = 3,
        Approved = 4,
        Ordered = 5,
    }
}