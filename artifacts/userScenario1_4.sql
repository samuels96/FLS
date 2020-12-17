#Scenario 1
CREATE UNIQUE INDEX brand_name ON brand ( name );

CREATE PROCEDURE get_products_by_brand_name
(IN brandName CHAR(50))
BEGIN
  SELECT * FROM product p
  JOIN brand B ON p.BrandID = B.ID
  WHERE brandName = B.Name;
END

#Scenario 2
CREATE PROCEDURE add_product_to_order_basket_and_return_product_price
(IN desiredProductID INT, IN userBasketID INT, IN desiredQuantity INT, OUT totalPrice INT)
BEGIN
    DECLARE productStock INT DEFAULT 0;
    DECLARE productPrice INT DEFAULT 0;
    DECLARE quantityInBasket INT DEFAULT 0;
    DECLARE futureTotalQuantity INT DEFAULT 0;
    DECLARE basketProductId INT DEFAULT NULL;

    SELECT Stock , Price
    INTO productStock, productPrice
    from product
    WHERE ID = desiredProductID;

    SELECT Quantity
    INTO quantityInBasket
    FROM basket_product bp
    WHERE desiredProductID = bp.ProductID AND
          userBasketID = BasketID;

    SET futureTotalQuantity = quantityInBasket + desiredQuantity;

    IF(futureTotalQuantity <= productStock) THEN
        SELECT ID
        INTO basketProductId
        from basket_product
        WHERE ProductID = desiredProductID;

        INSERT INTO basket_product VALUES (basketProductId, userBasketID, desiredProductID, desiredQuantity)
        ON DUPLICATE KEY UPDATE
            Quantity = futureTotalQuantity;

        SET totalPrice = desiredQuantity * productPrice;
    END IF;

    select @totalPrice;
END;
                   
#Scenario 3
CREATE EVENT IF NOT EXISTS christmas_discount_start
ON SCHEDULE EVERY 1 YEAR
    STARTS '2020-12-12 00:00:00'
DO
    UPDATE product
    SET product.Discount = 20
    WHERE ID <> NULL;

CREATE EVENT IF NOT EXISTS christmas_discount_end
    ON SCHEDULE EVERY 1 YEAR
        STARTS '2020-12-20 00:00:00'
    DO
    UPDATE product
    SET product.Discount = 0
    WHERE ID <> NULL;

#Scenario 4
CREATE PROCEDURE get_products_by_category
(IN categoryName VARCHAR(50))
BEGIN
    SELECT * FROM catalogueProductView cpv
    WHERE categoryName = cpv.Category;
END;

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

CREATE PROCEDURE reset_order_basket
(IN orderID VARCHAR(50))
BEGIN
    DELETE FROM order_basket ob
    WHERE ob.orderID = orderID;
END;

CREATE TABLE orderAudit
(
    OrderID    INT,
    OrderDate  DATE,
    CustomerID INT,
    OldStatus VARCHAR(100),
    NewStatus VARCHAR(100),
    StatusChanged DATE
);

CREATE TABLE productStockAudit
(
    ProductID int,
    OldStock VARCHAR(100),
    NewStock VARCHAR(100),
    Date DATE
);

CREATE TRIGGER on_product_insert
    AFTER INSERT
    ON product FOR EACH ROW
    BEGIN
        INSERT INTO productStockAudit
        VALUES (New.ID, 0, New.Stock, CURDATE());
    END;

CREATE TRIGGER on_product_stock_change
    AFTER UPDATE
    ON product FOR EACH ROW
    BEGIN
        IF OLD.Stock <> NEW.Stock THEN
            INSERT INTO productStockAudit
            VALUES (New.ID, Old.Stock, NEW.Stock, CURDATE());
        END IF;
    END;

CREATE TRIGGER on_new_order
    AFTER INSERT
    ON `order` FOR EACH ROW
    BEGIN
        INSERT INTO orderAudit VALUES
        (NEW.ID, NEW.Date, NEW.CustomerID, NULL, NEW.Status, CURDATE());
    END;

CREATE TRIGGER on_order_status_change
    AFTER UPDATE
    ON `order` FOR EACH ROW
    BEGIN
        IF OLD.Status <> NEW.Status THEN
            INSERT INTO orderAudit VALUES
            (NEW.ID, NEW.Date, NEW.CustomerID, OLD.Status, NEW.Status, CURDATE());
        END IF;
    END;

CREATE TRIGGER on_order_payment_complete
    AFTER UPDATE
    ON `order` FOR EACH ROW
    BEGIN
        IF New.IsPaid = TRUE THEN
            UPDATE product p
            INNER JOIN order_basket ob ON New.ID = ob.orderID
            INNER JOIN product_basket_joint pbj ON pbj.BasketID = ob.ID
            SET
                p.Stock = p.Stock - pbj.Quantity
            WHERE p.ID = pbj.ProductID;
        END IF;
    END;
