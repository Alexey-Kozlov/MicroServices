FROM node:18-alpine as builder

WORKDIR /app

COPY package.json .
ENV REACT_APP_IDENTITY=/identity
ENV REACT_APP_FRONT=/

RUN npm install

COPY . .

RUN npm run build

FROM nginx

RUN apt update \ 
  && apt install -y mc

COPY --from=builder /app/build /usr/share/nginx/html/ms/