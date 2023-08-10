FROM mcr.microsoft.com/devcontainers/base:bookworm

ARG DOTNETSDK_VERSION=7.0
ARG INSTALL_DIR=/home/vscode/dotnetsdk

RUN apt update &&\
apt -y full-upgrade &&\
apt -y install entr &&\
wget https://dot.net/v1/dotnet-install.sh -O dotnet-install.sh &&\
chmod +x ./dotnet-install.sh &&\
./dotnet-install.sh --channel $DOTNETSDK_VERSION --install-dir $INSTALL_DIR &&\
chown -R vscode $INSTALL_DIR &&\
chgrp -R vscode $INSTALL_DIR

ENV DOTNET_ROOT=$INSTALL_DIR
ENV PATH=$PATH:$DOTNET_ROOT:$DOTNET_ROOT/tools
ENV DOTNET_CLI_TELEMETRY_OPTOUT=true
