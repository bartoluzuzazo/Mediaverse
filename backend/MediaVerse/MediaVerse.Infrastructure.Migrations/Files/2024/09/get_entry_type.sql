CREATE OR REPLACE FUNCTION get_entry_type (e_id uuid)
RETURNS text AS $type$
declare
    selected_entry int;
BEGIN

SELECT 1 INTO selected_entry FROM Book e
WHERE e.id = e_id;

IF selected_entry IS NOT NULL THEN
        return 'Book';
END IF;

SELECT 1 INTO selected_entry FROM Movie e
WHERE e.id = e_id;

IF selected_entry IS NOT NULL THEN
    return 'Movie';
END IF;

SELECT 1 INTO selected_entry FROM Song e
WHERE e.id = e_id;

IF selected_entry IS NOT NULL THEN
    return 'Song';
END IF;

SELECT 1 INTO selected_entry FROM Album e
WHERE e.id = e_id;

IF selected_entry IS NOT NULL THEN
    return 'Album';
END IF;

SELECT 1 INTO selected_entry FROM Game e
WHERE e.id = e_id;

IF selected_entry IS NOT NULL THEN
    return 'Game';
END IF;

SELECT 1 INTO selected_entry FROM Series e
WHERE e.id = e_id;

IF selected_entry IS NOT NULL THEN
    return 'Series';
END IF;

SELECT 1 INTO selected_entry FROM Episode e
WHERE e.id = e_id;

IF selected_entry IS NOT NULL THEN
    return 'Episode';
END IF;

return 'Entry';

END;
$type$ LANGUAGE plpgsql IMMUTABLE;