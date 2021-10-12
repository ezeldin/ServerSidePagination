WITH Data_CTE 
AS
(
    SELECT 
        product_id as Id,
        product_name as ProductName,
	    model_year as ProductYear,
        list_price as Price
From production.products 
WHERE (
         product_name = @ProductName
         OR @ProductName IS NULL
      )      
), 
Count_CTE 
AS 
(
        SELECT COUNT(1) AS TotalCount From Data_CTE
)
SELECT * FROM Data_CTE CROSS JOIN Count_CTE

ORDER BY Id asc OFFSET @PageNo ROWS FETCH NEXT @PageSize ROWS ONLY;