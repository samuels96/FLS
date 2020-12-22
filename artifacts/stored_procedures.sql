CREATE PROCEDURE get_products_by_brand_name
(IN brandName CHAR(50))
BEGIN
  SELECT * FROM product p
  JOIN brand B ON p.BrandID = B.ID
  WHERE brandName = B.Name;
END

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
  
CREATE PROCEDURE get_products_by_category
(IN categoryName VARCHAR(50))
BEGIN
    SELECT * FROM catalogueProductView cpv
    WHERE categoryName = cpv.Category;
END;


CREATE PROCEDURE reset_order_basket
(IN orderID VARCHAR(50))
BEGIN
    DELETE FROM order_basket ob
    WHERE ob.orderID = orderID;
END;
                    
CREATE
    DEFINER = admin@`%` PROCEDURE create_customer(IN fName VARCHAR(50), IN lName VARCHAR(50), IN email VARCHAR(50),
                                                  IN address VARCHAR(50))
BEGIN
	INSERT INTO ESHOP.customer (FirstName, LastName, Email, Phone, Address)
	VALUES ('@fName','@lName','@email','@address');
END;
                                                                    
CREATE
    DEFINER = admin@`%` PROCEDURE delete_product(IN id1 INT)
BEGIN
	DELETE FROM ESHOP.product a
    WHERE a.ID = id1;
END;

CREATE
    DEFINER = admin@`%` PROCEDURE login_verification(IN email1 VARCHAR(16))
BEGIN
	SELECT email, password FROM ESHOP.admins a
    WHERE a.email = email1;
    
END;

