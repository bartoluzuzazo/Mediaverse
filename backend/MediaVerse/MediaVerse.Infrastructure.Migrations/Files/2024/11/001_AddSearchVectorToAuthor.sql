ALTER TABLE author ADD COLUMN search_vector tsvector
    GENERATED ALWAYS AS (to_tsvector('english', coalesce(name, '') || ' ' || coalesce(surname, ''))) STORED;

CREATE INDEX author_ts_idx ON author USING GIN (search_vector);