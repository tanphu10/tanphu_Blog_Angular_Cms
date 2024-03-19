import { Component } from '@angular/core';
import { AdminApiTestApiClient } from 'src/app/api/admin-api.service.generated';

@Component({
  selector: 'app-post',
  templateUrl: './post.component.html',
})
export class PostComponent {

  constructor(private testApi:AdminApiTestApiClient) { }
test(){
  this.testApi.testAuthen().subscribe({
    next:()=>{
      console.log('ok')
    },
    error:(error:any)=>{
      console.log("xảy ra lỗi nè",error);
    }
  })

}
}
