pipeline {
    agent {
        docker {
            image 'mcr.microsoft.com/dotnet/core/sdk'
            label 'linux'
        }
    }
    stages {
        stage('Build') {
            steps {
                sh 'dotnet build src'
            }
        }
        stage('Test') {
            steps {
                sh 'dotnet test src --no-build'
            }
        }
        stage('Deploy') {
            steps {
                sh 'dotnet publish src -c Release -f netcoreapp3.1 --self-contained -r linux-x64 -o src/Web/bin/Release/publish'
                sh 'curl -L "https://packages.cloudfoundry.org/stable?release=linux64-binary&source=github" | tar -zx'
                sh 'mv cf /usr/local/bin'
                sh 'cf login -a https://api.run.pcfone.io'
                sh 'cf push'
            }
        }
    }
}
