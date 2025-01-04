ALTER TABLE article ADD COLUMN search_vector tsvector
    GENERATED ALWAYS AS (to_tsvector('english', coalesce(title, '') || ' ' || coalesce(lede, ''))) STORED;

CREATE INDEX article_ts_idx ON article USING GIN (search_vector);
