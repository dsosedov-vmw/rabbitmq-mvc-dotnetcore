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
                sh 'dotnet --version'
            }
        }
        stage('Test') {
            steps {
                echo 'Testing...'
            }
        }
        stage('Deploy') {
            steps {
                echo 'Deploying...'
            }
        }
    }
}
