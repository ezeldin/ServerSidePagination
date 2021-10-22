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
      and
      (
         list_price = @Price
         OR @Price IS NULL
      )
)
SELECT * FROM Data_CTE 
