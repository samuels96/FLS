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
