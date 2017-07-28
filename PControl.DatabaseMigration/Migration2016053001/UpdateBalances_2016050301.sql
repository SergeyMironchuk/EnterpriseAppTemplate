/****** Object:  StoredProcedure [dbo].[UpdateBalances]    Script Date: 31.05.2016 11:43:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[UpdateBalances] 
      @Quantity float,
      @Date datetime,
      @Product_Id int
AS
BEGIN
	SET NOCOUNT ON;

      declare @LastDate datetime;
      declare @LastQuantity float;

      select @LastDate=[Date], @LastQuantity=Quantity
      from Balances
      where [Date] = (select max([Date]) from Balances where Date <= @Date and Product_Id = @Product_Id) and Product_Id = @Product_Id;

      if @LastDate is NULL
        INSERT INTO Balances (Quantity, [Date], Product_Id) VALUES (0, @Date, @Product_Id)
      else if @LastDate  < @Date
        INSERT INTO Balances (Quantity, [Date], Product_Id) VALUES (@LastQuantity, @Date, @Product_Id)

      UPDATE Balances set Quantity = Quantity + @Quantity WHERE [Date] >= @Date and Product_Id = @Product_Id;
END

GO