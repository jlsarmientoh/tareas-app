FROM gradle:6.9-jdk8

WORKDIR /tmp
ADD . /tmp

RUN gradle build

CMD ["gradle", "clean", "bootRun"]
EXPOSE 3000
