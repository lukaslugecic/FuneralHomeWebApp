import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FuneralService } from 'src/app/services/funeral/funeral.service';

@Component({
  selector: 'app-funeral-detail',
  templateUrl: './funeral-detail.component.html',
  styleUrls: ['./funeral-detail.component.scss']
})
export class FuneralDetailComponent implements OnInit {
  funeralId: number | undefined;

  constructor(
    private readonly _funeralService: FuneralService,
    private readonly route: ActivatedRoute,
    ) { }

  ngOnInit(): void {
    this.funeralId = Number(this.route.snapshot.paramMap.get('id'));
    console.log(this.funeralId);
    this._funeralService.getFuneralDetailById(this.funeralId).subscribe({
      next: (res) => {
        console.log(res);
      }
    });
  }

}
