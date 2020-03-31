import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { UsersService } from '../services/users.service';

@Component({
  selector: 'app-other-users-profile',
  templateUrl: './other-users-profile.component.html',
  styleUrls: ['./other-users-profile.component.css']
})
export class OtherUsersProfileComponent implements OnInit {

  isUserLoggedIn: boolean = false;
  selectedUser: any;

  constructor(private router: Router, private route: ActivatedRoute, private usersService: UsersService) { }

  ngOnInit() {
    this.usersService.isUserLoggedIn().subscribe(() => {
      this.isUserLoggedIn = true;
      let id = this.route.snapshot.params['Id'];
      this.usersService.getUserData(id).subscribe(response => {
        this.selectedUser = response.json().user;
      });
    }, () => {
      this.usersService.raiseLogoutEvent();
      this.isUserLoggedIn = false;
    });
  }


  onCancel() {
    this.router.navigate(['/all_profiles']);
  }

}
