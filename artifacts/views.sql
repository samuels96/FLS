CREATE VIEW catalogueProductView
AS
    SELECT
        c.Name Category,
        p.Name, p.Price, p.Discount, p.Stock, p.Description, p.imageUrl,
        b.Name BrandName, b.LogoURL BrandLogo, b.Description BrandDescription
    FROM
        product p
    INNER JOIN brand b ON p.BrandID = b.ID
    INNER JOIN category c ON p.Category = c.ID;

CREATE VIEW orderAndOrderBasketView
AS
    SELECT
        o.ID OrderID, o.Date, o.DeliveryAddress, o.ExpectedDeliveryDate,
        o.Status, o.IsPaid,
        ob.ID OrderBasketID,
        pbj.ID ProductBasketJointID, pbj.ProductID, pbj.Quantity
    FROM
         `order` o
    INNER JOIN order_basket ob ON o.ID = ob.orderID
    INNER JOIN product_basket_joint pbj ON pbj.BasketID = ob.ID;
