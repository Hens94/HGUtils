﻿namespace HGUtils.Common.Enums
{
    public enum ResultType
    {
        Success = 0,
        WithWarning = 111,
        WithNoContent = 222,
        NotFound = 333,
        BadRequest = 444,
        DatabaseError = 777,
        ThirdPartyError = 888,
        ApiError = 999
    }
}
