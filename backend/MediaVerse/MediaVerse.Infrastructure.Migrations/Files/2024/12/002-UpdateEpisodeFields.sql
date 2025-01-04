DROP TABLE game_developer;
DROP TABLE developer;

alter table episode
    add season_number integer not null default 1;

alter table episode
    add episode_number integer not null default 1;

