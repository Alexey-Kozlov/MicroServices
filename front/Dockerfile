FROM node:18-alpine as builder

ENV REACT_APP_IDENTITY=/ms/identity/api
ENV REACT_APP_LOGIN=/ms/identity
ENV REACT_APP_FRONT=/ms
ENV REACT_APP_MAIN=/ms/api
ENV REACT_APP_TOKEN_NAME=msjwt

WORKDIR /app

COPY package.json .

RUN npm install

COPY . .

RUN npm run build

FROM nginx

RUN apt update \ 
  && apt install -y mc

COPY --from=builder /app/build /usr/share/nginx/html

COPY default.conf /etc/nginx/conf.d/