WITH Data_CTE 
AS
(
    SELECT 
        product_id as Id,
        product_name as ProductName,
	    model_year as ProductYear,
        list_price as Price
From production.products 
)
SELECT * FROM Data_CTE 
