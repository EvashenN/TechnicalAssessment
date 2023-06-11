USE [Northwind]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[pr_GetOrderSummary] 
@StartDate datetime, 
@EndDate datetime, 
@EmployeeID int = NULL, 
@CustomerID varchar(5) = NULL
AS 
BEGIN

Select  
CONCAT(e.TitleOfCourtesy,' ',e.FirstName, ' ', e.LastName) AS EmployeeFullName,
s.CompanyName AS 'Shipper CompanyName' ,
c.CompanyName AS 'Customer CompanyName',
COUNT(DISTINCT o.OrderID) AS NumberOfOrders,
CONVERT(VARCHAR(11), o.OrderDate, 106) AS 'Date',
SUM(o.Freight) AS TotalFreightCost,
COUNT(DISTINCT od.ProductID) AS NumberOfDifferentProducts,
ROUND(SUM(od.Quantity * od.UnitPrice * (1 - od.Discount)),2) AS TotalOrderValue   

FROM Orders o
 Join Shippers s 
on o.ShipVia = s.ShipperID
 Join Employees  e
on o.EmployeeID = e.EmployeeID
 Join Customers c
on o.CustomerID = c.CustomerID
 Join [Order Details] od
on o.OrderID = od.OrderID 

WHERE
o.OrderDate >= @StartDate AND 
o.OrderDate <= @EndDate AND
(@EmployeeID is null OR o.EmployeeID = @EmployeeID) AND
(@CustomerID is null OR o.CustomerID = @CustomerID)

GROUP BY 
o.OrderDate,
e.TitleOfCourtesy,
e.FirstName,
e.LastName,
c.CustomerID,
c.CompanyName,
S.CompanyName


END
GO


