CREATE OR REPLACE FUNCTION full_text_search_entries(search_term TEXT)
    RETURNS SETOF entry
AS
$$
BEGIN
    RETURN QUERY
        SELECT entry.*
        FROM entry,
             to_tsquery('english', search_term) query,
             NULLIF(ts_rank(to_tsvector(entry.name), query), 0) rank_name,
             NULLIF(ts_rank(to_tsvector(entry.description), query), 0) rank_description,
             SIMILARITY(search_term, entry.name) similarity
        WHERE query @@ entry.search_vector
           OR similarity >= 0.2
        ORDER BY rank_name, rank_description, similarity DESC NULLS LAST;
END

$$ LANGUAGE plpgsql;
