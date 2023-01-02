env.DOCKER_IMAGE_VERSION='v1.0.0'
env.DOCKER_NAME_DEV='dev.web.api'
env.DOCKER_NAME_TEST='test.web.api'

pipeline {
    agent any 
   
    stages {  
        stage('Build Image') {
            when { 
                changeset "web.api/**"
                branch 'develop'
            }
            steps {
               sh 'docker build -t  $DOCKER_NAME_DEV:$DOCKER_IMAGE_VERSION.${BUILD_ID} -f Dockerfile.dev .'
               sh 'docker rm -f $DOCKER_NAME_DEV || true'
               sh 'docker run -d --name $DOCKER_NAME_DEV -p 8888:80 $DOCKER_NAME_DEV:$DOCKER_IMAGE_VERSION.${BUILD_ID}'
            }
        }

        stage('master') {
            when { branch 'master' }
            steps {
               sh 'docker build -t $DOCKER_NAME_TEST:$DOCKER_IMAGE_VERSION.${BUILD_ID} -f Dockerfile.dev .'
               sh 'docker rm -f $DOCKER_NAME_TEST || true'
               sh 'docker run -d --name $DOCKER_NAME_TEST -p 9999:80  $DOCKER_NAME_TEST:$DOCKER_IMAGE_VERSION.${BUILD_ID}'
            }
        }


       stage('changeset test'){
          when { changeset "infrustruture/**"}
            steps {
                sh 'echo library changed'
            }
        }      
        
        stage('End of Job'){   
            steps {
                sh 'echo ------------ The end of build job------------'
            }
        } 
    }
}