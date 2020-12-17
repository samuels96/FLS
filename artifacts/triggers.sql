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