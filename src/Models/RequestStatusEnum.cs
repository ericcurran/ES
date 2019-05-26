namespace Models
{
    public enum RequestStatusEnum
    {
        SavedToAzure = 1,
        Captured     = 2,
        Pending      = 3,
        Approved     = 4
    }

    public enum RequestTypeEnum
    {
        Peer = 1,
        TBD = 2,
        IME = 3,
        IMEO = 4,
        Fee = 5,
        Feep = 6,
        Court = 7
    }


}