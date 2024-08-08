import { Component, Input, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { UploadService } from '../../services/upload.service'

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent{

  saftFile: File | undefined;
  enable: boolean = false


  constructor(private route: ActivatedRoute, private upRest: UploadService, private router: Router,) { }

  onFileSelected(event: any) {
    this.saftFile = event.target.files[0];
    this.enable = true;
    console.log(this.saftFile);
  }

  onSubmit() {
    if (this.saftFile) {
      const formData = new FormData();
      formData.append('file', this.saftFile);

      this.upRest.executeSaft(formData).subscribe(
        (data) => {
          console.log('Success:', data);
          localStorage.setItem("ready", 'true');
          this.router.navigate(['/calendar']).then(() => {
            window.location.reload();
          });
        },
        (error) => {
          console.error('Error:', error);
        }
      );
    }
  }


}
