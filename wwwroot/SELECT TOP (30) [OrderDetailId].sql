SELECT TOP (30) [OrderDetailId]
      ,[OrderId]
      ,[ProductId]
      ,[UnitPrice]
      ,[Quantity]
      ,[Discount]
  FROM [Northwind24_93_HVR].[dbo].[OrderDetails]
  ORDER BY [OrderDetailId] DESC