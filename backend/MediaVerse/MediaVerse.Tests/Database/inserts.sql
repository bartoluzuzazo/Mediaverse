-- EXAMPLE DATA

INSERT INTO role
VALUES ((SELECT uuid_in('e345fcaf-a7a0-4663-bdc3-6b498f598693')), 'User');

INSERT INTO role
VALUES ((SELECT uuid_in('aa2da3d4-6710-4714-9bc8-054023063118')), 'Administrator');


INSERT INTO profile_picture
VALUES ((SELECT uuid_in('dbc2478f-ee4f-492e-afdd-7584a81e2baa')),
        PG_READ_BINARY_FILE('/data/tom.jpg'));

INSERT INTO "user"
VALUES ((SELECT uuid_in('af7c1fe6-d669-414e-b066-e9733f0de7a8')), 'admin', 'admin@admin.com',
        'AQAAAAIAAYagAAAAEFcLklIh16tE1VkAsBVmTnZjmItN+JNiGenKPkmg6AFfyQEhU7ns26B3Oym3pHhacA==', -- hasło: admin
        uuid_in('dbc2478f-ee4f-492e-afdd-7584a81e2baa'));

INSERT INTO "user"
VALUES ((SELECT uuid_in('08c71152-c552-42e7-b094-f510ff44e9cb')), 'user', 'user@admin.com',
        'AQAAAAIAAYagAAAAEFcLklIh16tE1VkAsBVmTnZjmItN+JNiGenKPkmg6AFfyQEhU7ns26B3Oym3pHhacA==', -- hasło: admin
        uuid_in('dbc2478f-ee4f-492e-afdd-7584a81e2baa'));

INSERT INTO "role_user"
VALUES ((SELECT uuid_in('aa2da3d4-6710-4714-9bc8-054023063118')), (SELECT uuid_in('af7c1fe6-d669-414e-b066-e9733f0de7a8')));

-- inserting an entry
INSERT INTO cover_photo
VALUES ((SELECT uuid_in('02ec0136-8beb-4481-9004-dd6fd2ad4072')),
        PG_READ_BINARY_FILE('/data/typewriter.jpg'));

INSERT INTO entry
VALUES ((SELECT uuid_in('2cd11d9b-51f4-4f0b-96a4-7e65881438ec')),
        'The catcher in the rye',
        'Lorem ipsum dolor sit amet',
        '05-27-1951',
        (SELECT uuid_in('02ec0136-8beb-4481-9004-dd6fd2ad4072')));

INSERT INTO book_genre
VALUES ((SELECT uuid_in('6fa89aba-d05d-4507-bdb3-b29c88b4c133')), 'Classics');
INSERT INTO book_genre
VALUES ((SELECT uuid_in('5ae2a462-d28c-413b-9a4a-a5937e4cbaaf')), 'Fiction');

INSERT INTO book
VALUES ((SELECT uuid_in('2cd11d9b-51f4-4f0b-96a4-7e65881438ec')), '978-9-9162-1269-1', 'Lorem ipsum dolor sit amet');

INSERT INTO book_book_genre
VALUES ((SELECT uuid_in('2cd11d9b-51f4-4f0b-96a4-7e65881438ec')),
        (SELECT uuid_in('6fa89aba-d05d-4507-bdb3-b29c88b4c133')));
INSERT INTO book_book_genre
VALUES ((SELECT uuid_in('2cd11d9b-51f4-4f0b-96a4-7e65881438ec')),
        (SELECT uuid_in('5ae2a462-d28c-413b-9a4a-a5937e4cbaaf')));


INSERT INTO entry
VALUES ((SELECT uuid_in('d13d7237-0e1a-402e-abec-e34bf4945d34')),
        'Another book',
        'Lorem ipsum dolor sit amet',
        '05-27-1961',
        (SELECT uuid_in('02ec0136-8beb-4481-9004-dd6fd2ad4072')));

INSERT INTO book
VALUES ((SELECT uuid_in('d13d7237-0e1a-402e-abec-e34bf4945d34')), '978-9-9162-1269-1', 'Lorem ipsum dolor sit amet');


INSERT INTO author
VALUES ((SELECT uuid_in('5f49e424-f74e-49c9-8d82-67a4202354df')), 'Jerome David', 'Salinger',
        'A newyorker notorious for his reclusive nature', NULL,
        (SELECT uuid_in('dbc2478f-ee4f-492e-afdd-7584a81e2baa')));

INSERT INTO author
VALUES ((SELECT uuid_in('7cfcec2b-0d68-4e91-857a-1016e8f50bb2')), 'Jed', 'Bartlet',
        'Former president of the United states and a recipient of the economics Nobel prize', NULL,
        (SELECT uuid_in('dbc2478f-ee4f-492e-afdd-7584a81e2baa')));



