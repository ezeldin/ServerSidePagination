/*DECLARE @IdentityNumber NVARCHAR(50) = NULL,
        @IdentityTypeId INT = NULL,
        @PageNo INT = 0,
        @PageSize INT = 10;*/

DECLARE @SqlCommand NVARCHAR(MAX),
        @whereClause NVARCHAR(MAX),
        @query NVARCHAR(MAX),
        @joins NVARCHAR(MAX),
        @orderBySection NVARCHAR(MAX),
        @Select NVARCHAR(MAX),
        @Columns NVARCHAR(MAX),
        @CountColumn NVARCHAR(MAX),
        @CountQuery NVARCHAR(MAX),
        @FromTable NVARCHAR(MAX);

SET @Select = N' select ';

SET @CountColumn = ' Count(1) as TotalCount ';

SET @Columns
    = N' Req.CreationDate,
        Req.Iban,
        Res.SRN,
        Res.CRN,
        LUTAVSAccountStatus.NameAr as AccountStatus,
        LUTAVSCreditStatus.NameAr as CreditStatus,
        LUTAVSMatchStatus.NameAr as MatchStatus,
        Res.CallbackStatus,
        P.IdentityNumber,
        P.IdentityTypeId,
        P.Name,
        Res.IsNeedCallBack,
        Res.HttpStatus  
 ';

SET @FromTable = N' From AVSServiceRequest Req ';

SET @joins
    = N' join Party P on Req.PartyId=P.PartyId
        left join AVSServiceResponse Res on Req.Id=Res.AVSServiceRequestId  
        left join LUTAVSAccountStatus on res.AccountStatus=LUTAVSAccountStatus.code
        left join LUTAVSCreditStatus on res.CreditStatus=LUTAVSCreditStatus.code
        left join LUTAVSMatchStatus on res.MatchStatus=LUTAVSMatchStatus.code
 ';

SET @whereClause = N' 
		WHERE 1=1 ';


IF (@IdentityNumber IS NOT NULL)
BEGIN
    SET @whereClause = @whereClause + N' AND  (@IdentityNumber = P.IdentityNumber) ';
END;
IF (@IdentityTypeId IS NOT NULL)
BEGIN
    SET @whereClause = @whereClause + N' AND  (@IdentityTypeId = P.IdentityTypeId ) ';
END;
IF (@SendDateFrom IS NOT NULL)
BEGIN
    SET @whereClause = @whereClause + N' AND  (Req.CreationDate >= @SendDateFrom) ';
END;
IF (@SendDateTo IS NOT NULL)
BEGIN
    SET @whereClause = @whereClause + N' AND  (Req.CreationDate < dateadd(DAY,1,@SendDateTo) ) ';
END;
IF (@IBAN IS NOT NULL)
BEGIN
    SET @whereClause = @whereClause + N' AND  (@IBAN = Req.Iban) ';
END;


SET @orderBySection = N' 
 ORDER BY 1 Desc OFFSET @PageNo ROWS FETCH NEXT @PageSize ROWS ONLY
OPTION (MAXDOP 1)
;';

SET @CountQuery = CONCAT(@Select, @CountColumn, @FromTable, @joins, @whereClause);

DECLARE @Parms NVARCHAR(MAX)
    = N'
		@IdentityNumber NVARCHAR(50) ,
	    @IdentityTypeId INT,
        @SendDateFrom NVARCHAR(50),
        @SendDateTo NVARCHAR(50),
        @IBAN NVARCHAR(50),
	    @PageNo INT,
	    @PageSize INT';


EXECUTE sp_executesql @CountQuery,
                      @Parms,
                      @IdentityNumber,
                      @IdentityTypeId,
                      @SendDateFrom,
                      @SendDateTo,
                      @IBAN,
                      @PageNo,
                      @PageSize;

SET @query = CONCAT(@Select, @Columns, @FromTable, @joins, @whereClause, @orderBySection);

EXECUTE sp_executesql @query,
                      @Parms,
                      @IdentityNumber,
                      @IdentityTypeId,
                      @SendDateFrom,
                      @SendDateTo,
                      @IBAN,
                      @PageNo,
                      @PageSize;