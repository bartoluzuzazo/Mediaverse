ARG VITE_API_ADDRESS='https://mediaverse-fzgmcvh4h5h2ddhk.westeurope-01.azurewebsites.net:80'

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

EXPOSE 8080

ENTRYPOINT  ["serve", "-s",  "-l",  "8080",  "dist"]