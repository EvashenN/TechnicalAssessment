USE [Northwind]
GO

DECLARE @RC int
DECLARE @StartDate datetime
DECLARE @EndDate datetime
DECLARE @EmployeeID int
DECLARE @CustomerID varchar(5)

EXECUTE @RC = [dbo].[pr_GetOrderSummary] 
   @StartDate = N'1 jan 1996'
  ,@EndDate = N'31 aug 1996'
  ,@EmployeeID = NULL
  ,@CustomerID = NULL

GO


