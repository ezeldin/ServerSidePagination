WITH Data_CTE 
AS
(
    SELECT 
        Party.Name,
        Req.NotifyRefId,
	    Req.IdentityId as IdentityNumber,
        Req.IdentityTypeId,
        Req.CreationDate,
        Req.SendStatus,
        Res.IBAN as Iban,
        Res.IdentityId as UpdatedIdentityNumber,
        Res.IdentityTypeId as UpdatedIdentityTypeId,
        Res.CreationDate as UpdateCreationDate
From AVSNotifyRequest Req
        left join AVSNotifyResponse Res on Req.Id=Res.AVSNotifyRequestId
        left join Party on Req.PartyId=Party.PartyId
WHERE (
         Req.IdentityId = @IdentityNumber
         OR @IdentityNumber IS NULL
      )
      AND
      (
          Req.IdentityTypeId = @IdentityTypeId
          OR @IdentityTypeId IS NULL
      )
	  AND
      (
          Req.SendStatus = @SendStatus
          OR @SendStatus IS NULL
      )
	  AND
      (
			(@UpdateStatus=1 and Res.CreationDate is not null) 
			or (@UpdateStatus=2 and Res.CreationDate is null)  
			OR @UpdateStatus IS NULL
      )
       AND
      (
          Req.NotifyRefId =@NotifyRefId
          OR @NotifyRefId IS NULL
      )
	  AND
	  (
         Res.IdentityId = @UpdaterIdentityNumber
         OR @UpdaterIdentityNumber IS NULL
      )
      AND
      (
          Res.IdentityTypeId = @UpdaterIdentityTypeId
          OR @UpdaterIdentityTypeId IS NULL
      )
	  AND
      (
          Req.CreationDate >= @SendDateFrom
          OR @SendDateFrom IS NULL
      )
	  AND
      (
          Req.CreationDate < dateadd(DAY,1,@SendDateTo)
          OR @SendDateTo IS NULL
      )
	  AND
      (
          Res.CreationDate >= @UpdateDateFrom
          OR @UpdateDateFrom IS NULL
      )
	  AND
      (
          Res.CreationDate < dateadd(DAY,1,@UpdateDateTo)
          OR @UpdateDateTo IS NULL
      )
), 
Count_CTE 
AS 
(
        SELECT COUNT(1) AS TotalCount From Data_CTE
)
SELECT * FROM Data_CTE CROSS JOIN Count_CTE

ORDER BY CreationDate DESC OFFSET @PageNo ROWS FETCH NEXT @PageSize ROWS ONLY;