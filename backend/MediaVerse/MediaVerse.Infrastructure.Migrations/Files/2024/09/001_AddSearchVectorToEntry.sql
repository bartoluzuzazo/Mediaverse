ALTER TABLE entry ADD COLUMN search_vector tsvector
    GENERATED ALWAYS AS (to_tsvector('english', coalesce(description, '') || ' ' || coalesce(name, ''))) STORED;

CREATE INDEX entry_ts_idx ON entry USING GIN (search_vector);