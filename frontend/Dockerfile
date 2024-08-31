FROM node:lts-alpine AS build-stage

WORKDIR /usr/src/app

COPY . .

RUN npm install && npm run build

FROM node:lts-alpine

WORKDIR /usr/src/app


COPY --from=build-stage /usr/src/app/dist /usr/src/app/dist

RUN npm install -g serve

RUN adduser -D fe_user

USER fe_user

EXPOSE 3000

ENTRYPOINT  ["serve", "-s",  "-l",  "3000",  "dist"]