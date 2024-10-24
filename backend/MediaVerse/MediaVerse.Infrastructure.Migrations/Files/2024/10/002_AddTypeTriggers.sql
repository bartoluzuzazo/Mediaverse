CREATE OR REPLACE FUNCTION set_entry_type() RETURNS TRIGGER
    LANGUAGE plpgsql
AS
$$
BEGIN
    IF (SELECT type FROM entry e WHERE e.id = new.id) IS NOT NULL THEN
        RAISE EXCEPTION 'Entry type cannot be changed';
end if;

UPDATE entry e SET type = tg_argv[0]::text
    WHERE e.id = new.id;
RETURN new;
END;
$$;

CREATE OR REPLACE TRIGGER book_type_trigger
    AFTER INSERT OR UPDATE ON book
                                  FOR EACH ROW EXECUTE PROCEDURE set_entry_type('Book');

CREATE OR REPLACE TRIGGER movie_type_trigger
    AFTER INSERT OR UPDATE ON movie
                                  FOR EACH ROW EXECUTE PROCEDURE set_entry_type('Movie');

CREATE OR REPLACE TRIGGER series_type_trigger
    AFTER INSERT OR UPDATE ON series
                                  FOR EACH ROW EXECUTE PROCEDURE set_entry_type('Series');

CREATE OR REPLACE TRIGGER episode_type_trigger
    AFTER INSERT OR UPDATE ON episode
                                  FOR EACH ROW EXECUTE PROCEDURE set_entry_type('Episode');

CREATE OR REPLACE TRIGGER game_type_trigger
    AFTER INSERT OR UPDATE ON game
                                  FOR EACH ROW EXECUTE PROCEDURE set_entry_type('Game');

CREATE OR REPLACE TRIGGER song_type_trigger
    AFTER INSERT OR UPDATE ON song
                                  FOR EACH ROW EXECUTE PROCEDURE set_entry_type('Song');

CREATE OR REPLACE TRIGGER album_type_trigger
    AFTER INSERT OR UPDATE ON album
                                  FOR EACH ROW EXECUTE PROCEDURE set_entry_type('Album');


CREATE OR REPLACE FUNCTION lock_entry_type() RETURNS TRIGGER
    LANGUAGE plpgsql
AS
$$
BEGIN
    IF new.type <> old.type THEN
        RAISE EXCEPTION 'Entry type cannot be changed manually';
END IF;
RETURN new;
END;
$$;

CREATE OR REPLACE TRIGGER entry_type_trigger
    AFTER INSERT OR UPDATE ON entry
                                  FOR EACH ROW EXECUTE PROCEDURE lock_entry_type();
