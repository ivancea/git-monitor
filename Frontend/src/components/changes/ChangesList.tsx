import React from 'react';
import { ListGroup } from 'reactstrap';
import { ChangeWrapper } from '../../types/changes';
import { Change } from './Change';
import './changes.scss';

type Props = {
  changes: ChangeWrapper[];
};

export function ChangesList({ changes }: Props): React.ReactElement {
  return (
    <>
      <ListGroup>
        {changes
          .map((change, index) => <Change key={index} change={change} />)
          .reverse()}
      </ListGroup>
    </>
  );
}
