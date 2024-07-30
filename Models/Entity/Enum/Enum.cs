namespace HotelManagementSystem.Model.Entity.Enum
{
    public enum RoomStatus
    {
        CheckedIn = 1,
        CheckedOut,
        Pending,
        Cancelled,

    }
    

    public enum BedType
    {
        KingBed = 1,
        TrundleBed,
        RollawayBed,
    }

    public enum RoomType
    {
        Single = 1,
        Double,
        Suite
    }

    public enum Gender
    {
        Male =1,
        Female 
    }

    public enum Review
    {
        Excellent = 1,
        Good ,
        Bad ,
        Worst 

    }

    public enum PaymentMethod
    {
        Cash = 1,
        Transfer
    }

    public enum PaymentStatus
    {
        Pending = 1,
        Approved
    }

    public enum UserRole
    {
        Admin = 1,
        Customer
    } 
    
    
    public enum RoomAvailability
    {
        Available = 1,
        NotAvailable 
    }
}


