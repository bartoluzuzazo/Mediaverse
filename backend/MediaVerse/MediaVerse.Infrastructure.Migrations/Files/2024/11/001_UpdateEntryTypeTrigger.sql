CREATE OR REPLACE FUNCTION set_entry_type() RETURNS TRIGGER
    LANGUAGE plpgsql
AS
$$
DECLARE
    e_type varchar(50);
BEGIN
    SELECT type INTO e_type FROM entry e WHERE e.id = new.id;
    IF e_type IS NOT NULL AND e_type != tg_argv[0] THEN
        RAISE EXCEPTION 'Entry type cannot be changed';
end if;

UPDATE entry e
SET type = tg_argv[0]::text
    WHERE e.id = new.id;
RETURN new;
END;
$$;