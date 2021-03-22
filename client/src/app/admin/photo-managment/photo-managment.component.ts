import { Component, OnInit } from '@angular/core';
import { Photo } from 'src/app/_models/photo';
import { AdminService } from 'src/app/_services/admin.service';

@Component({
  selector: 'app-photo-managment',
  templateUrl: './photo-managment.component.html',
  styleUrls: ['./photo-managment.component.css']
})
export class PhotoManagmentComponent implements OnInit {
  photos: Photo[];
  //bdModalRef: BsModalRef;

  constructor(private adminService: AdminService) { }

  ngOnInit(): void {
    this.getPhotosForApproval();
  }

  getPhotosForApproval(){
    this.adminService.getPhotosForApproval().subscribe( photos =>{
      this.photos = photos;
    });
  }

  rejectPhoto(photoId){
    this.adminService.rejectPhoto(photoId).subscribe(() => {
      let photoIndex = this.photos.findIndex(p => p.id === photoId);
      this.photos.splice(photoIndex, 1);
    })
  }

  approvePhoto(photoId){
    this.adminService.approvePhoto(photoId).subscribe(() => {
      let photoIndex = this.photos.findIndex(p => p.id === photoId);
      this.photos.splice(photoIndex, 1);
    })
  }
}
