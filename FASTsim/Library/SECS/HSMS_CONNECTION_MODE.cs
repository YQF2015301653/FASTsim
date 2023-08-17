namespace FASTsim.Library.SECS
{
    public enum HSMS_CONNECTION_MODE
    {
        ACTIVE,
        PASSIVE,
        ALTERNATING
    }

    public enum HSMSState
    {
        NOT_CONNECTED = 0,
        CONNECTED = 1,
        NOT_SELECTED = 2,
        SELECTED = 3
    }

    public enum SType : byte
    {
        Message = 0,
        SelectReq = 1,
        SelectResp = 2,
        DeselectReq = 3,//Deselect shall not be used in an HSMS-SS implementation
        DeselectResp = 4,
        LinktestReq = 5,
        LinktestResp = 6,
        RejectReq = 7,
        NotUsed = 8,
        SeparateReq = 9
    }
}

