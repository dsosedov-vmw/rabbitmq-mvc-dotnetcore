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
                echo 'Deploying...'
            }
        }
    }
}
